#!/usr/bin/env python
import os
import sys

from django.template import Context, Template
from django.template.loader import get_template


class Table:
    def __init__(self, tableName):
        self.name = tableName
        self.columns = []

class DatabaseDoc:
    def __init__(self, databaseName):
        self.name = databaseName
        self.tables = []
        self.functions = []
        self.views = []
        self.triggers = []
        self.sequences = []
        self.types = []


if __name__ == "__main__":
    os.environ.setdefault("DJANGO_SETTINGS_MODULE", "pg_dgen.settings")

    t = get_template('doc.jade')

    table = Table("AboutProduct")
    table.columns = ['ProductId','Name','SomeDate','Version','StatusType']

    db = DatabaseDoc("MppPoc")
    db.tables.append(table)
    db.tables.append(table)


    c = Context({"db" : db })
    print (t.render(c))

