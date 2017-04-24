#!/bin/bash
# Get namespace
namespace=$(grep 'name: ' 00-namespace.yaml | cut -f4 -d ' ')

# Apply all yaml files
for file in `ls *.yaml`; do kubectl create -f $file --namespace $namespace --record; done
