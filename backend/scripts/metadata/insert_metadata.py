from python_graphql_client import GraphqlClient
import pandas as pd
from numpy import nan

def main():
    #Read metadata from local csv file
    metadata = pd.read_csv("data/metadata.csv")
    #Replace all the Pandas NaN to python None type
    metadata = metadata.replace({nan: None})
    print("Starting script")
    print("*************************")
    print(metadata)
    print("*************************")

    client = GraphqlClient(endpoint='http://localhost:5000/')

    query = """
        mutation addMetadata($someMetadata : MetadataInput!){
            addMetadata(newMetadata:$someMetadata){
                 metadataID 
            }
        }
    """
    variables = {}
    for i in range(0, len(metadata.index)):
        variable = {
        "someMetadata":{
            "name":metadata['name'][i],
            "number":metadata['number'][i],
            "serialNumber":metadata['serialNumber'][i],
            "signal" :metadata['signal'][i],
            "voltage" : metadata['voltage'][i],
            "lending":(str(metadata['lending'][i]) == "True") if metadata['lending'][i] else None,
            "lendingPrice": float(metadata['lendingPrice'][i]) if metadata['lendingPrice'][i] else None,
            "cableLength": float(metadata['cableLength'][i]) if metadata['cableLength'][i] else None,
            "tollerance":(str(metadata['tollerance'][i]) == "True") if metadata['tollerance'][i] else None,
            "checkOnInspectionRound": (str(metadata['checkOnInspectionRound'][i]) == "True") if metadata['checkOnInspectionRound'][i] else None,
            "measureArea":metadata['measureArea'][i],
            "modelNumber": metadata['modelNumber'][i],
            "ownerID": metadata['ownerID'][i],
            "company": metadata['company'][i],
            "department": metadata['department'][i],
            "identificator": metadata['identificator'][i],
            "tag1": metadata['tag1'][i],
            "tag2": metadata['tag2'][i],
            "tag3": metadata['tag3'][i],
            "locationDescription":metadata['locationDescription'][i],
            "altitude": int(metadata['altitude'][i]) if metadata['altitude'][i] else None,
            "coordinate": metadata['coordinate'][i],
            "purchaseDate": metadata['purchaseDate'][i],
            "plannedDisposal":  metadata['plannedDisposal'][i],
            "actualDisposal": metadata['actualDisposal'][i],
            "nextService":  metadata['nextService'][i],
            "warrantyDate":  metadata['warrantyDate'][i],
            "website": metadata['website'][i],
            "timeless": (str(metadata['timeless'][i]) == "True") if metadata['timeless'][i] else None,
            "servicePartner": metadata['servicePartner'][i]
            }
        }
        variables[i] = variable
    row_counter = 0
    err_counter = 0
    print("Beginning to query Database")
    for variable in variables:
        data = client.execute(query=query, variables=variables[variable])
        try:
            data["data"]["addMetadata"]["metadataID"]
        except:
            print("Error with metadata row nr " + str(row_counter))
            print(data)
            err_counter = err_counter + 1
        row_counter = row_counter + 1

    if err_counter == 0:
        print("All metadata added with success.")
    else:
        print(str(err_counter) + " rows could not be inserted. ")

if __name__ == "__main__":
    main()
