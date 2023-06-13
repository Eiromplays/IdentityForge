#!/bin/bash

openssl genrsa -out identityforge-ca.private-key.pem 4096
openssl rsa -in identityforge-ca.private-key.pem -pubout -out identityforge-ca.public-key.pem
openssl req -new -x509 -key identityforge-ca.private-key.pem -out identityforge-ca.cert.pem -days 365 -config identityforge-ca.cnf
openssl pkcs12 -export -inkey identityforge-ca.private-key.pem -in identityforge-ca.cert.pem -out identityforge-ca.pfx -passout file:Password.txt
openssl x509 -in identityforge-ca.cert.pem -out identityforge-ca.crt

openssl genrsa -out identityserver.private-key.pem 4096
openssl rsa -in identityserver.private-key.pem -pubout -out identityserver.public-key.pem
openssl req -new -sha256 -key identityserver.private-key.pem -out identityserver.csr -config identityserver.cnf
openssl x509 -req -in identityserver.csr -CA identityforge-ca.cert.pem -CAkey identityforge-ca.private-key.pem -CAcreateserial -out identityserver.cer -days 365 -sha256 -extfile identityserver.cnf -extensions req_v3
openssl pkcs12 -export -inkey identityserver.private-key.pem -in identityserver.cer -out identityserver.pfx -passout file:Password.txt

openssl genrsa -out frontend.private-key.pem 4096
openssl rsa -in frontend.private-key.pem -pubout -out frontend.public-key.pem
openssl req -new -sha256 -key frontend.private-key.pem -out frontend.csr -config frontend.cnf
openssl x509 -req -in frontend.csr -CA identityforge-ca.cert.pem -CAkey identityforge-ca.private-key.pem -CAcreateserial -out frontend.cer -days 365 -sha256 -extfile frontend.cnf -extensions req_v3
openssl pkcs12 -export -inkey frontend.private-key.pem -in frontend.cer -out frontend.pfx -passout file:Password.txt

# Original Source: https://github.com/NCarlsonMSFT/CertExample/blob/master/Certs/CreateCerts.sh