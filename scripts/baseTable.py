import os
import psycopg2
from psycopg2 import sql


def create_base_tables(host, dbname, user, password):
    with psycopg2.connect(dbname=dbname, user=user, password=password, host=host) as conn:
        with conn.cursor() as cursor:
            cursor.execute(sql.SQL("""
                CREATE TABLE IF NOT EXISTS users ( 
                id SERIAL PRIMARY KEY,
                name text,
                template jsonb)
                """))
            print('The "{table}" table was successfully created.'.format(table='users'))
  
