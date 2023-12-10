# Thought process

I decided to use Sqlite + Dapper for data storage. Firstly, because I haven't tried this combination
before. Secondly, because Sqlite is quite lightweight and doesn't require any setup, so I can omit
any running instructions - it should work out of the box. Thirdly, because I just like Dapper for
simple use cases.

I'm not sure I would advise to use Sqlite this way in production, but one SQL database can be easily
substituted by a different one. The overall principle is the same.

After some experimentation with layers, I decided to keep the usual:
1. Presentation - controllers. Just the logic that has to do with forming the actual HTTP
responses goes here.
2. Business. A layer where things like validation and paycheck calculation live.
I subdivided this into Services and Logic. Logic is where I put the purely computational Paycheck
calculator. The rest goes into Services.
3. Data. Talk to the actual database.

I also removed the intermediate models (Dependant, Employee). I found that I didn't need anything
more complex than the DTOs. If the application were a bit more complex than a simple CRUD API,
I think I would have left it in; but now it seemed to be some code for the sake of code. I even find
some of the service methods unnecessary; they just transfer the calls to the data layer; but I decided
to leave these in, because I really dislike the controllers directly calling into the data layer.

I've done my best to implement the requirement that went with this challenge; I'm not sure if I correctly
understood some of the benefit calculations, or indeed how the paycheck is supposed to look like. In
real life I would probably go and clarify some things before finalizing the implementation.

The whole thing is still a bit of a WIP; I've done as much as I could in one evening of coding. There are
some obvious tests missing, and some obvious validations, and some obvious endpoints. Still, I think this
gives a decent view of what the API would look like eventually.