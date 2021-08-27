#!/usr/bin/python2.9
# -*-coding:Latin-1 -*
import pandas as pa
from releves import Releves
import pyodbc
releves = pa.read_csv('import202102.csv', ";")
# print(releves)
# listOfReleves= [(Releves(row.get('ReferenceContrat'), row.get('numcompteur'),
#  row.get('Ancienindex'), row.get('Nouvelindex'), row.get('consommation'), 
# row.get('datereleve'), row.get('periode'), row.get('nombrejours'), row.get('categorie')
# )) for index, row in releves.iteritems() ]  

# print(releves[0])

# sql_conn = pyodbc.connect("""DRIVER={ODBC Driver 13 for SQL Server};
#                             SERVER=win-sqlsrv;
#                             DATABASE=PROERA_TEST;
#                             UID=eradbread;
#                             PWD=passer;
#                             Trusted_Connection=yes""") 

# cursor = sql_conn.cursor()

server = 'win-sqlsrv' 
database = 'PROERA_TEST' 
username = 'eradbread' 
password = 'passer' 
cnxn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER='+server+';DATABASE='+database+';UID='+username+';PWD='+ password)
cursor2 = cnxn.cursor()


for index,row in releves.iterrows():
    cursor2.execute("""INSERT INTO dbo.releves([Reference Contrat],numcompteur, [Ancien index], [Nouvel index], consommation, [date de relève], periode, nbreJour, categorie) 
                         values (?, ?,?, ?, ?, ?, ?, ?, ?)""", row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8] ) 
    cnxn.commit()
cursor2.close()
cnxn.close()