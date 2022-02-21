### Install chart

```bash
kubectl create namespace offlogs-test
helm install test --namespace offlogs-test .
```

### Minikube help

```bash
minikube kubectl -- logs <podname> -n offlogs-test
```
