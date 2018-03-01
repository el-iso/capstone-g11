from MongoInterface import *
from Device import Device
import datetime
import time
import pymongo
import sys, argparse

storage_db, storage_col = get_database_and_collection("SavedData", "Save2")
target_db, target_col = get_database_and_collection("Production", "LiveData")
drop_collection(target_col)
config = get_collection_config(storage_col)
results_0 = search_by_device_id(storage_col, 0)
all_results = search_by_device_ids(storage_col, config["devices"])

for poll_step in range(max([len(x) for x in all_results.values()])):
	for messages in all_results.values():
		try:
			message = messages[poll_step]
			message[alias_rcv] = datetime.datetime.now().timestamp()
			insert_json(target_col, messages[poll_step], verbose=True)
			if config["config"]["async"] != 0:
				time.sleep(config["config"]["poll"] / config["config"]["devices"])
		except:
			pass
	if config["config"]["async"] == 0:
		time.sleep(config["config"]["poll"])

