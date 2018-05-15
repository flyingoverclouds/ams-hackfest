for a local dev test, use 'ngrok' to publicly expose the local HTTP endpoint thru a ngrok tunnel : 
	C:\> ngrok http -host-header=localhost 7071

Create or update the EventGridTopic Subscription with the https forwarding url returned by ngrok 
(this is a public endpoint of the tunnel pointing to your function)