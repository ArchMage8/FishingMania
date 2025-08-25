Hmm? #speaker: Takeshi #trigger: next
Did you need something?
->main

== main ==
What are you doing? #speaker: Iris #trigger: next

Waiting... #speaker: Takeshi #trigger: next

Ehm? Waiting for what exactly? #speaker: Iris #trigger: next

Well customers I guess, but as you can see not many people by the docks right now... #speaker: Takeshi #trigger: next

Customers? Are you a shopkeeper? #speaker: Iris #trigger: next

No...? #speaker: Takeshi #trigger: next
Do you see a shop anywhere?
I'm a sailor, I use that boat over there to get people to the other islands...

Wait so you can take me to other islands? #speaker: Iris #trigger: next

Ehm..., isn't that what I said? #speaker: Takeshi #trigger: next

So can you take to the mainland then? #speaker: Iris #trigger: next

What? No! #speaker: Takeshi #trigger: next
You need a ship to get that far, the boat here is just a small vessel...

Ah... #speaker: Iris #trigger: next
Are there ships on the other islands then? #speaker: Iris #trigger: next

Probably..., There are bigger shipyards anyway. #speaker: Takeshi #trigger: next

Can you take me there? #speaker: Iris #trigger: next

You got a ticket? #speaker: Takeshi #trigger: next

    +[A ticket?]
        ->EndA

    +[No?]
        ->EndB

=== EndA ===
Yes a ticket... 
->EndFinal

=== EndB ===
Hmmm...
->EndFinal

=== EndFinal ===
You need to buy a ticket to use the boat...

Ah ok then! #speaker: Iris #trigger: next
->END