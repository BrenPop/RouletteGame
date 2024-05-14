Steps to setup this project

1. Clone the repo locally
2. cd into the RouletteGame directory
3. Open the solution in Visual Studio
4. Clean and build the project
5. Run the project
6. To test the project you can use the swagger page, or Postman

API Endpoints:

BetController:

Endpoint: /api/V1/Bet

GET Request - Get bets history

POST Request -	Create/Place new bet.
				request body required amount and colour values, colour must be "Red", "Green", or "Black"

Endpoint: api/v1/Spin

GET Request - Get spins history

POST Request - Create new spin to used for payouts

Endpoint: api/v1/Payout

GET Request - Get payouts history

POST Request -	Create new payout.
				This process includes retrieving all bets with the status "Placed"
				Fetching the latest spin
				Calculating the payout for each bet, updating each bet with the spin and payout values
				Returning a list of payouts and their amounts
