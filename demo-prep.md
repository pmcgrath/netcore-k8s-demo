```
Browser - Firefox
	Start slides
		cd ~/oss/github.com/pmcgrath/netcore-k8s-demo
		present 
		Open browser http://127.0.0.1:3999/k8s.slide

	Start minikube dashboard
		minikube dashboard

New tab
	cd ~/oss/github.com/pmcgrath/netcore-k8s-demo/k8s


Split window
	http://conemu.github.io/en/SplitScreen.html
	powershell -new_console:sV

Text Size
	Top right -> Settings -> 
		Main -> Size 24
		Features -> Colors -> Schemes -> <Solarized Light>

Top pane 
	function prompt() { (split-path $pwd -leaf) + " > " }
	Use this for kubectl

	Start bash
In bottom pane
	export PS1='$(basename $(pwd)) > '
	After initial deployment
```
		minikube service webapi --url
		svc_base_url=$(minikube service webapi --url)

		while [[ true ]]; do curl -w '\n' ${svc_base_url}/environment; sleep 1s; done
```
