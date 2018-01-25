from MongoInterface import *
from ComPortScan import get_port
import serial
from xbee import XBee, ZigBee
import json
import datetime

serial_port = serial.Serial(get_port(), 9600)
xbee = ZigBee(serial_port)

def extractJSON(raw):
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



oldT = datetime.datetime.now()
timeElapsed = 0
set_collection("radio_test")
#drop_collection()
while True:
	#print("Blah")
	try:
		data = xbee.wait_read_frame()["rf_data"]
		#print("Received")
		data = extractJSON(data)
		print(data)
		data = json.loads(data)
		#print(data)
		newT = datetime.datetime.now()
		delta = newT - oldT
		oldT = newT
		timeElapsed += delta.total_seconds()
		if delta.total_seconds() > 10:
			timeElapsed = 0
			drop_collection()
		data[alias["receive"]] = timeElapsed
		print(data)
		insert_json(data)
	except KeyboardInterrupt:
		break

serial_port.close()