from MongoInterface import *
from Device import Device
import time
import argparse

parser = argparse.ArgumentParser(description="Simulate receiving radio signals from multiple devices")
parser.add_argument('-p', '--poll', type=float, default=0.5, help="How often (in seconds) each simulated device will send data [FLOAT, default=0.5]")
parser.add_argument('-n', '--devices', type=int, default=1, help="How many devices will be simulated [INT, default=1]")
parser.add_argument('-o', '--offset', type=int, default=0, help="DeviceIDs will begin incrememting at this number [INT, Default=0]")

args = parser.parse_args()
print(args)
print(args.devices)

def main():
	drop_database()

	device_list = []
	for i in range(args.devices):
		device_list.append(Device(args.offset + i))

	for d in device_list:
		d.garble = 0.0
		d.drop = 0.0

	while(True):
		for d in device_list:
			message = d.NextMessage()
			#print(message)
			insert_json(message)
		time.sleep(args.poll)
		

main()