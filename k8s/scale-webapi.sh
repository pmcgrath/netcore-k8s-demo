#!/bin/bash
# Parameters
count=${1:-'3'}

# Get namespace
namespace=$(grep 'name: ' 00-namespace.yaml | cut -f4 -d ' ')

# Scale
kubectl scale deployments/webapi --replicas=${count} --namespace $namespace
