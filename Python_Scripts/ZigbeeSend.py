from MongoInterface import *
from ComPortScan import get_port
from Device import Device
import serial
import time
from xbee import XBee, ZigBee

poll_interval = 0.5

ser = serial.Serial(get_port(), 9600)
xbee = ZigBee(ser)

def main():
	d = Device(0)

	while(True):
		message = bytes(str(d.NextMessage()).encode('utf-8'))
		ser.write(message)
		print(message)
		time.sleep(poll_interval)

main()