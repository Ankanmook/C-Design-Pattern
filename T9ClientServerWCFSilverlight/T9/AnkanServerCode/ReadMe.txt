-------------------
What to do
-------------------

(i) Go to AnkanServerCode\T9Service\T9Service\bin\Debug
(ii) Run 9Service.exe as administrator.
(iii) The prompt shows the link to connect to obtain WSDL. Same is useful in client to obtain the service reference.
(iv) The url link is http://localhost:3355/T9Service.svc/

----------------------
Service contract
----------------------

Initialization performs building the datastruture:

(i) [OperationContract]
List<string> GetData(Int64 key);

The following API is used to send Int64 number which user presses in the key '2' to '9'. 
The number is read by server and the server returns complete possible words with respect to the number sent. 
If the word is not available then the server returns a list with only one string which has '---' 
The number of '-' is equal to length of the Int64 number sent. For example 667854 returns "------".