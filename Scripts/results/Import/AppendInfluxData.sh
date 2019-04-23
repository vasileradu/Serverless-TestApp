#!/bin/sh

fileCount=0;
index=1;

FILES=./influx*
for f in $FILES
do
  fileCount=$((fileCount + 1))
done

echo "Importing [$fileCount] files into influxdb..."
echo "----------------------------------------"

for f in $FILES
do
  echo "Processing $f file...[ $index / $fileCount ]"
  
  index=$((index + 1))
  
  curl -i -XPOST "http://localhost:8086/write?db=TestApp&precision=ns" --data-binary @$f
done

echo "Processed [$fileCount] files"
echo "----------------------------------------"