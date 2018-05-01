from MongoInterface import *
from Device import Device
import time
import datetime
import argparse

parser = argparse.ArgumentParser(description="Simulate receiving radio signals from multiple devices")
parser.add_argument('-p', '--poll', type=float, default=0.5, help="How often (in seconds) each simulated device will send data [FLOAT, default=0.5]")
parser.add_argument('-n', '--devices', type=int, default=1, help="How many devices will be simulated [INT, default=1]")
parser.add_argument('-o', '--offset', type=int, default=0, help="DeviceIDs will begin incrememting at this number [INT, Default=0]")
parser.add_argument('-a', '--async', type=int, default=0, help="If 0: Devices send data at same time, Else: Devices send data evenly through time[INT(BOOL), default=0]")
parser.add_argument('-q', '--quick', type=int, default=0, help="If 0: Will insert in real time, Else: Will insert in real time[INT(BOOL), Default=0]")
parser.add_argument('-l', '--length', type=float, default=36000.0, help="Length of Time (seconds) that will be simulated [FLOAT, Default=100.0]")
parser.add_argument('-s', '--save', type=str, default="", help="Collection to save data to (Database=SavedData)")

args = parser.parse_args()
print(args)

def main():
	if args.save != "":
		db, data_col = get_database_and_collection("SavedData", args.save)
	else:
		db, data_col = get_database_and_collection("Production", "LiveData")
	drop_collection(data_col)
	make_default_indexes(data_col)

	insert_json(data_col, {"config":vars(args), "devices":[args.offset+i for i in range(args.devices)]})	

	device_list = []
	for i in range(args.devices):
		device_list.append(Device(args.offset + i))

	message_counter = 0
	for i in range(int(1.0/args.poll * args.length)):
		try:
			for d in device_list:
				message = d.NextMessage()
				message[alias_rcv] = datetime.datetime.now().timestamp()
				insert_json(data_col, message, verbose=True)
				if args.quick == 0:
					if(args.async == 0):
						if(d == device_list[-1]):
							time.sleep(args.poll)
					else:
						time.sleep(args.poll / args.devices)
		except KeyboardInterrupt as k:
			print("##################################################\nEnding\n##################################################")
			break
		message_counter+=1

main()