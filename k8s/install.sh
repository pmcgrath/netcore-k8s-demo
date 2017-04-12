#!/bin/bash
set -e
set -o pipefail

# Commands
cmd-install-latest-kubectl() {
	# See http://kubernetes.io/docs/user-guide/prereqs/
	version=$(curl -s https://storage.googleapis.com/kubernetes-release/release/stable.txt)
	echo "Installing kubectl version $version"
	cd /tmp
	curl -LO https://storage.googleapis.com/kubernetes-release/release/${version}/bin/linux/amd64/kubectl
	chmod +x kubectl
	sudo mv kubectl /usr/local/bin/
	cd -
}

cmd-install-minikube() {
	# See https://github.com/kubernetes/minikube/releases
	local version=0.18.0

	# Overrides, see http://wiki.bash-hackers.org/howto/getopts_tutorial
	while getopts “:v” option; do
		case $option in
			v) version=$OPTARG ;;
			\?) exit 1 ;;
		esac
	done

	curl -Lo /tmp/minikube https://storage.googleapis.com/minikube/releases/v${version}/minikube-linux-amd64 && chmod +x /tmp/minikube && sudo mv /tmp/minikube /usr/local/bin/
}

cmd-help() {
	echo
	cat <<-EOF
	Help:
    	$script kubectl
        	Installs or updates to the latest kubectl
    	$script minikube [-v version]
	        Installs or updates minikube
    	$script help
        	Show help
EOF
}

# Main
script="./${0##*/}"
command="$1"
case "$1" in
	kubectl|ctl) shift;cmd-install-latest-kubectl "$@" ;;
	minikube|mk) shift;cmd-install-minikube "$@" ;;
	*) command="help";cmd-help "$@" ;;
esac
exit 0
