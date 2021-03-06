import baseTable as baseTable

if __name__ == '__main__':
	#
	host = input("host: ")
	dbname = input("dataBase: ")
	user = input("user: ")
	password = input("password: ")
	#
	baseTable.create_base_tables(dbname=dbname, user=user, password=password, host=host)