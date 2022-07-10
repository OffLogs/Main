## How it works?

### Quick info

1. Your website (or service) generates logs or debug information
2. These logs, using the OffLogs client (or your client), must be sent to the API at https://api.offlogs.com
3. OffLogs encrypts all incoming information
4. Now by going to https://offlogs.com/dashboard you can view all received logs in a convenient interface

### Data encryption

Your services may send information that requires increased security requirements,
therefore OffLogs encrypts all received information.

#### Detailed information
1. After registration, you will receive a file with a private and public key. The private key is only with you.

> Attention! If this file is lost then you will not be able to access your account and your data.
> We store data and keys only in **encrypted** form and this information can only be **decrypted using your private key**.
> This means that if you lose the key, **you will not be able to access your account**.

2. We store a public key that allows you to encrypt information, but does not allow you to decrypt it.

3. After OffLogs receives information from your services, it will immediately encrypt it

4. Now in the control panel OffLogs will decrypt information while viewing logs
