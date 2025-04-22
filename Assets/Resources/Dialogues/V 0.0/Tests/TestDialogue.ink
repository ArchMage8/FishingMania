Welcome to the the restaurant! #speaker: 1 #trigger: loopback
-> main

=== main ===
What would you like to order?
    +[Fish Pasty]
        I would like a Fish Pasty #speaker: 2 #trigger: next
        -> chosen("Fish Pasty", 10)
    +[Fish Pie]
        I would like a Fish Pie #speaker: 3 #trigger: next
        -> chosen("Fish Pie", 20)

=== chosen(dish, cookTime) ===
Ok i will serve {dish} in {cookTime} minutes  #speaker: 4 #trigger: next

Would you like something else? #speaker: 5 #trigger: next
    +[Yes]
        OK Then!
        #trigger: loopback   #speaker: 5A
        ->Section1
    
        
    +[No]
        Alright, please wait for your order #trigger: next   #speaker: 5B
        ->END 

=== Section1 ====
->main
