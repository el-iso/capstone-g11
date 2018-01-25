from MongoInterface import *
import random
import json

heartMin = 0
heartMax = 200
bloodOxMin = 0
bloodOxMax = 100
respMin = 0
respMax = 100

bigSwing = 1.5
smallSwing = 0.66

bigChance = 0.1
smallChance = 0.4

heartValueRange = heartMax - heartMin
heartSwingRange = (0.01 * heartValueRange, 0.023 * heartValueRange)
bloodOxValueRange = bloodOxMax - bloodOxMin
bloodOxSwingRange = (0.001 * bloodOxValueRange, 0.0025 * bloodOxValueRange)
respValueRange = respMax - respMin
respSwingRange = (0.01 * respValueRange, 0.025 * respValueRange)

drop_code = "DROP"
garble_code = "GARBLE"
normal_code = "NORMAL"

def checkFormat(message):
	template = {alias_device:0, alias_sent:0, alias_heart:0.0, alias_blood:0.0, alias_resp:0.0}
	try:
		for k,v in message:
			if k not in template.keys():
				return False
			if type(v) != type(template[k]):
				return False
		return True
	except:
		return False

def garbled():
	result = {}
	for i in range(random.randint(0,5)):
		garbleText = ""
		for j in range(random.randint(1,3)):
			garbleText += chr(int(random.random() * 128))
		result[garbleText] = random.randint(-25, 25) * random.random()
	return result

def POS_NEG(value, data_range, tolerance=0.4, adjust=True):
	value_percentile = (data_range + value) / data_range - 1
	if value_percentile > 0.5-tolerance/2 or value_percentile < 0.5+tolerance/2:
		k = 0.5
	else:
		k = value_percentile
	
	if value < 0:
		k = 0.0
	
	result = -1 if random.random() < k else 1
	
	if adjust:
		if random.random() < bigChance:
			result *= bigSwing
		elif random.random() < smallChance:
			result *= smallSwing
	#print("k={0:.2f}".format(k)+"\trand={}\tresult={}".format(compare,result))
	return result

def swing(json):
	result = json
	if json.get(alias_heart):
		k = POS_NEG(json[alias_heart], heartValueRange, tolerance=0.3)
		adjustment = k * (random.random() * (heartSwingRange[1]-heartSwingRange[0]) + heartSwingRange[0])
		result[alias_heart] += adjustment
	else:
		result[alias_heart] = random.randrange(heartMin, heartMax)
	
	if json.get(alias_blood):
		k = POS_NEG(json[alias_blood], bloodOxValueRange)
		adjustment = k * (random.random() * (bloodOxSwingRange[1]-bloodOxSwingRange[0]) + bloodOxSwingRange[0])
		result[alias_blood] += adjustment
	else:
		result[alias_blood] = random.randrange(bloodOxMin, bloodOxMax)
	
	if json.get(alias_resp):
		k = POS_NEG(json[alias_resp], respValueRange)
		adjustment = k * (random.random() * (respSwingRange[1]-respSwingRange[0]) + respSwingRange[0])
		result[alias_resp] += adjustment
	else:
		result[alias_resp] = random.randrange(respMin, respMax)
	return result

def partialRandom(deviceID, messageNum, previousJSON, clean=False, garble=0.05, drop=0.1):
	result = {}
	if not clean:
		if random.random() < drop:
			return (drop_code, None)
		if random.random() < garble:
			return (garble_code, garbled())
	result[alias_device] = deviceID
	result[alias_sent] = messageNum
	nextValues = swing(previousJSON)
	result[alias_heart] = float("{0:.2f}".format(nextValues[alias_heart]))
	result[alias_blood] = float("{0:.2f}".format(nextValues[alias_blood]))
	result[alias_resp] = float("{0:.2f}".format(nextValues[alias_resp]))
	return (normal_code, result)

class Device:
	ID = 0
	messagesSent = 0
	latestValidMessage = {}
	garble = 0.05
	drop = 0.15

	def __init__(self, did):
		self.ID = did
		self.messagesSent = 0
		self.previousMessage = {}

	def Copy(self):
		other = Device(self.ID)
		other.messagesSent = self.messagesSent
		other.latestValidMessage = self.latestValidMessage
		other.garble = self.garble
		other.drop = self.drop
		return other

	def Copy_Settings(self, other):
		self.garble = other.garble
		self.drop = other.drop


	def NextMessage(self):
		response_code, response = partialRandom(self.ID, self.messagesSent, self.latestValidMessage, garble=self.garble, drop=self.drop)
		if response_code == normal_code:
			self.latestValidMessage = response
		self.messagesSent += 1
		return response