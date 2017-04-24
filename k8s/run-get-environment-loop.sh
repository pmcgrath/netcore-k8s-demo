#!/bin/bash
# Get namespace
namespace=$(grep 'name: ' 00-namespace.yaml | cut -f4 -d ' ')
# Get service name
service_name=$(grep 'name: ' 03-webapi-service.yaml | cut -f4 -d ' ')


# Get service base url - minikube IP and port
service_base_url=
while [[ -z "$service_base_url" ]]; do
	service_base_url=$(minikube service $service_name --namespace $namespace --url 2> /dev/null)
	[[ $? != 0 ]] && echo "$(date) Still waiting on the service url" && sleep 1s
done;


# Run infinite loop
while [[ true ]]; do
	curl -w '\n' ${service_base_url}/environment
	sleep .5s
done
