from pymongo import MongoClient

client = MongoClient('localhost', 27017)
db = client.randomdata
collection = db.randomTest

alias_device = "did"
alias_sent = "sent"
alias_heart = "h"
alias_blood = "b"
alias_resp = "r"
alias_rcv = "rcv"

expected_fields = ["did", "sent", "h", "b", "r", "rcv"]
defaults = {k:-1 for k in expected_fields}

def set_collection(c):
	global collection
	#print(collection)
	collection = db[c]
	#print(collection)

def set_database(d):
	client.db = d

def drop_database():
	#print(MongoClient().database_names())
	client = MongoClient('localhost', 27017)
	client.drop_database(db)
	#print(MongoClient().database_names())

def drop_collection():
	db.collection.drop()

def insert_json(json):
	print("Insert: {}".format(json))
	collection.insert_one(json)
	

def insert_many_json(jsonList):
	collection.insert_many(jsonList)

def make_default_indexes():
	collection.create_index([(alias_device, 1)], background=True)
	collection.create_index([(alias_sent, -1)], background=True)
	collection.create_index([(alias_device, 1), (alias_sent, -1)], background=True)

def make_index(indexFields):

	if len(collection.find({ index: { "$exists": true} })):
		collection.create_index(index)
	else:
		print("{} does not exist in database".format(index))

def search_by_device_id(id, limit=10000):
	results = []
	for result in collection.find( 
		filter={alias_device:id}, 
		projection={"_id":0},
		limit=limit,
		sort=[(alias_sent,1)]
	):
		results.append(result)
	return results

def search_by_device_ids(idList, limit=10000):
	results = {}
	for id in idList:
		results[id] = search_by_device_id(id, limit)
	return results.items

def search_by_time_range(timeStart, timeEnd, limit=10000):
	results = {}
	for result in collection.find( 
		filter = { alias_sent:{ "$gte": timeStart, "$lte": timeEnd } },
		projection = {"_id":0},
		limit = limit,
		sort = [ (alias_device, 1), (alias_sent, -1) ]
	):
		if not results.get( result[ alias_device ] ):
			results[ result[ alias_device ] ] = []
		results[ result[ alias_device ] ].append(result)
	return results




