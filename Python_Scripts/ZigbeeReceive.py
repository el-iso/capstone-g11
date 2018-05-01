from MongoInterface import *
from ComPortScan import get_port
import serial
from xbee import XBee, ZigBee
import json
import datetime

serial_port = serial.Serial(get_port(), 9600)
xbee = ZigBee(serial_port)

"""def extractJSON(raw):
	jsonStart = 0
	jsonEnd = 0
	for i, char in enumerate(raw):
		if char == "{" and not jsonStart:
			jsonStart = i
			break
	for i, char in enumerate(raw[::-1]):
		if char == "}" and not jsonEnd:
			jsonEnd = len(raw) - i
			break
	if jsonStart and jsonEnd:
		try:
			json.loads(raw[jsonStart:jsonEnd])
			return raw[jsonStart:jsonEnd]
		except:
			return str({"Error":"JSON1"})
	else:
		return str({})
"""

def main():
	db = get_database("Production")
	collection = get_collection(db, "EE_Lab_Test")
	data_wrapper = dict()
	while True:
		try:
			data = xbee.wait_read_frame()
			print(data)
			
			data_wrapper["rcv"] = datetime.datetime.now().timestamp()
			data_wrapper["data"] = data
			
			insert_json(collection, data_wrapper)
			data_wrapper = dict()
		except KeyboardInterrupt:
			serial_port.close()
			break
			
			
main();