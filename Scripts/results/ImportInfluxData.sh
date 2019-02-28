#!/bin/sh
curl -i -XPOST http://localhost:8086/query --data-urlencode "q=DROP DATABASE TestApp"

curl -i -XPOST http://localhost:8086/query --data-urlencode "q=CREATE DATABASE TestApp"

curl -i -XPOST "http://localhost:8086/write?db=TestApp&precision=ns" --data-binary @influx.out