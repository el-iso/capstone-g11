from MongoInterface import *
from Device import Device
import time

poll_interval = 0.5

def main():
	d = Device(0)
	d.garble = 0.0
	d.drop = 0.0

	while(True):
		drop_database()
		message = d.NextMessage()
		print(message)
		insert_json(message)
		time.sleep(poll_interval)
		

main()