from MongoInterface import *
import random
import json

heartMin = 0
heartMax = 200
bloodOxMin = 0
bloodOxMax = 100
respMin = 20
respMax = 100

bigSwing = 1.5
smallSwing = 0.33

bigChance = 0.1
smallChange = 0.2

heartValueRange = heartMax - heartMin
heartSwingRange = (0.01 * heartValueRange, 0.1 * heartValueRange)
bloodOxValueRange = bloodOxMax - bloodOxMin
bloodOxSwingRange = (0.01 * bloodOxValueRange, 0.1 * bloodOxValueRange)
respValueRange = respMax - respMin
respSwingRange = (0.01 * respValueRange, 0.1 * respValueRange)

def makeRandomData(deviceID, timeStamp):
	dropPercent = 0.1
	if random.random() < dropPercent:
		return None
	else:
		json = {alias_device:deviceID,
			alias_sent:timeStamp,
			alias_heart:random.randrange(heartMin, heartMax),
			alias_blood:random.randrange(bloodOxMin, bloodOxMax),
			alias_resp:random.randrange(respMin, respMax)
		}
	return json

def makeRandomDataForTimeRange(deviceID, timeStart=0, timeEnd=5, stepsPerSecond=1):
	result = []
	for i in range(timeEnd-timeStart):
		second = timeStart + i
		if stepsPerSecond <= 1:
			data = makeRandomData(deviceID, second)
			if data:
				result.append(data)
		else:
			for step in range(stepsPerSecond):
				subSecond = step / float(stepsPerSecond)
				data = makeRandomData(deviceID, second + subSecond)
				if data:
					result.append(data)
	return result

def prettyPrint(searchResult):
	print json.dumps(searchResult, indent=2, sort_keys=True)

def garble():
	result = {}:
	for i in range(random.randint(0,5)):
		garbleText = ""
		for j in range(random.randint(1,3)):
			garbleText += char(random.random() * 128)
			print garbleText
		result[garbleText] = randint(-25, 25) * random.random()
	return json

def POS_NEG(v, r, adjust=True)
	k = max(0, v/r) - 0.5
	result = -1 if k > 0 else 1
	if adjust:
		if random.random() < bigChance:
			result *= bigSwing
		elif random.random() < smallChance:
			result *= smallSwing
	return result

def swing(json):
	result = json
	if json.get(alias_heart):
		k = POS_NEG(json[alias_heart], heartValueRange)
		adjustment = k * random.randrange(heartSwingRange[0], heartSwingRange[1], 0.1)
		result[alias_heart] += adjustment
	else:
		result[alias_heart] = random.randrange(heartMin, heartMax)
	
	if json.get(alias_blood):
		k = POS_NEG(json[alias_blood], bloodOxValueRange)
		adjustment = k * random.randrange(bloodOxSwingRange[0], bloodOxSwingRange[1], 0.1)
		result[alias_blood] += adjustment
	else:
		result[alias_blood] = random.randrange(bloodOxMin, bloodOxMax)
	
	if json.get(alias_resp):
		k = POS_NEG(json[alias_resp], respValueRange)
		adjustment = k * random.randrange(respSwingRange[0], respSwingRange[1], 0.1)
		result[alias_resp] += adjustment
	else:
		result[alias_resp] = random.randrange(respMin, respMax)
	return result





def partialRandom(deviceID, messageNum, previousJSON, clean=False, garble=0.05, drop=0.1):
	result = {}
	if not clean:
		if random.random() < drop:
			return None
		if random.random() < garble:
			return garble()
	result[alias_device] = deviceID
	result[alias_sent] = messageNum
	nextValues = swing(previousJSON)
	result[alias_heart] = nextValues[alias_heart]
	result[alias_blood] = nextValues[alias_blood]
	result[alias_resp] = nextValues[alias_resp]
	return result



def main():
	drop_database()
	numDevices = 3
	random.seed()
	for deviceID in range(numDevices):
		json = makeRandomDataForTimeRange(deviceID, timeStart=0, timeEnd=5, stepsPerSecond=2)
		insert_many_json(json)
	for timeStamp in search_by_device_id(1):
		print timeStamp
	#prettyPrint(search_by_device_id(1))
	#prettyPrint(search_by_time_range(1.5, 9.9999))
	#print search_by_device_ids([1,2])

main()

