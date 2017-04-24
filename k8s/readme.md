# Guide
- See https://kubernetes.io/docs/tutorials/kubernetes-basics/
- For deployment updates and rollbacks, see https://ryaneschinger.com/blog/rolling-updates-kubernetes-replication-controllers-vs-deployments/



# Install minikube and kubectl
```
./install.sh minikube
./install.sh kubectl
```



# Start minikube
```
# Get latest version from
minikube get-k8s-versions

# Will use default which is virtualbox
version=1.6.0
minikube start --kubernetes-version $version -v 10 | tee minikube-start.log
kubectl cluster-info
```



# Start dashboard
```
# With minikube
minikube dashboard

# Kubectl
kubectl create -f https://rawgit.com/kubernetes/dashboard/master/src/deploy/kubernetes-dashboard.yaml
# This will block acting as a proxy with security credentials to access the k8s cluster dashboard - will beed to append the /ui/ suffix to see the dashboard
kubectl proxy
```


```
# Create namespace - Optional but including here anyway
namespace='k8s-demo-dev'
kubectl create -f ./00-namespace.yaml


# Create config map - InMemory initially 
kubectl create -f 01-webapi-store-configmap.yaml --namespace $namespace


# Create web api app deployment
kubectl create -f 02-webapi-deployment.yaml --namespace $namespace 


# Create web api app service
kubectl create -f 03-webapi-service.yaml --namespace $namespace 


# Test


# Create redis deployment and internal cluster redis service
kubectl create -f 04-webapi-redis-store-deploymnent.yaml --namespace $namespace 
kubectl create -f 05-webapi-redis-store-service.yaml --namespace $namespace 





```















kubectl get pod webapi --namespace $namespace --output yaml
kubectl get pod webapi --namespace $namespace --output json
kubectl describe pod po/webapi --namespace $namespace


















# Start first service version instance
- Assumes you have already build the docker images and pushed to dockerhub (Public)

```
# Create namespace and pods
namespace='dev'
kubectl create -f ./00-namespace.yaml
kubectl create -f ./01-webapi-deployment.yaml --namespace $namespace

# Query
# Pod ls
kubectl get pods --namespace $namespace
# Our pod
pod_name=$(kubectl get pods --selector 'app==webapi' --namespace $namespace --output 'jsonpath={.items[0].metadata.name}')
kubectl logs $pod_name --namespace $namespace --follow
kubectl logs $pod_name --namespace $namespace --follow --tail 100
kubectl describe po/$pod_name --namespace $namespace

# Now start service
kubectl create -f 02-webapi-service.yaml --namespace $namespace
kubectl get services --namespace $namespace
minikube service webapi --url --namespace $namespace

# Show all resources
kubectl get all --namespace $namespace

# Show all resources in all namespaces
kubectl get all --all-namespaces
```



# Start using the service - ideally so we could see side by side with alterations - watch the hostName value
```
service_url=$(minikube service webapi --url --namespace $namespace)
while [ true ]; do date; curl -s -w '\n\n' ${service_url}/environment; sleep 1; done
```



# Start redis store - singular instance behind the service

kubectl apply -f  03-webapi-redis-store-deploymnent.yaml --namespace $namespace
kubectl apply -f  04-webapi-redis-store-service.yaml --namespace $namespace

