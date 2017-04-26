#!/bin/sh
seq=1
while [[ true ]]; do
	echo "${seq} $(date) working"
	sleep .5s	
	let seq=$((seq + 1))
done
