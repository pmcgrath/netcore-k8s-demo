#!/bin/bash
# Get namespace
namespace=$(grep 'name: ' 00-namespace.yaml | cut -f4 -d ' ')

# Apply all yaml files
kubectl set image deployments/webapi webapi=pmcgrath/webapi:2.0 --namespace $namespace --record
