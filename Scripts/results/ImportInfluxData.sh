#!/bin/sh

echo "Removing TestApp data from influxdb..."
echo "----------------------------------------"

curl -i -XPOST http://localhost:8086/query --data-urlencode "q=DROP DATABASE TestApp"

curl -i -XPOST http://localhost:8086/query --data-urlencode "q=CREATE DATABASE TestApp"

fileCount=0;

FILES=./influx*
for f in $FILES
do
  fileCount=$((fileCount + 1))
done

echo "Importing [$fileCount] files into influxdb..."
echo "----------------------------------------"

for f in $FILES
do
  echo "Processing $f file..."
  curl -i -XPOST "http://localhost:8086/write?db=TestApp&precision=ns" --data-binary @$f
done

echo "Processed [$fileCount] files"
echo "----------------------------------------"

