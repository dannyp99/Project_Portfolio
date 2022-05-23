# Automation-things

Programs I have made to automate tasks and jobs that I have to do.

## Mileage Reimbursement

- driving.c is a multithreaded c program that can be used to calculate the mileage reimbursement of multiple trips. The threading simply calculates the cost of all trips in parallel.

```bash
./driving 9 .58 8
```

For the above we have a valid way of running the program, first we write the name of the executable ./driving.
Next, we pass the parameters **\<number of trips\>** **\<reimbursement rate\>** **\<Number of miles\>**

Run the command and it will give you the total of how much the total reimbursement for that person is for that trip.
We also allow multiple trips with the same number of trips i.e.

```bash
./driving 4 .58 8 12 4
```

Again we run ./driving and pass:
**\<number of trips\>** **\<reimbursement rate\>** **\<Number of miles\>** **\<Number of miles\>** **\<Number of miles\>** ...
