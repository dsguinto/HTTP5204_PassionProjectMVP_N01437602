# HTTP5204 Passion Project MVP: Up-Next
**This is the final submission version of my MVP for my HTTP5204 Passion Project: Up-Next Store**

For my passion project, my goal was to create an ecommerce website called "Up-Next", which would allow users to post vintage/streetwear items that they want to sell, while also allowing other users to add these items to an order that they could purchase. 

I was able to complete the administrative side of the website, which allows a user with an admin account to be able to view the entire user and product database. The admin is able to view, add, edit, and delete users/products to the website. 

### Features Included
* Search
* Image Uploading
* HTML/CSS Styling (Responsiveness)

### Issues During Development
Unfortauntely, I was unable to implement the user account integration as I had hoped too. Ideally, a user that logins in with a "regular" account would be able to add products. However, the edit and delete features would only be accessible for products that the regular user owns. If they do not own the product, they can only view the product's details and add the product to a cart/order. A regular user would not have access to add, edit, or delete other users. They would only be able to view other users accounts (as well as all the products they are selling), as well as edit/delete their own account. I had made an User table early on in development, and only realized later that I should have used the AspNetUser's table instead. I hope to add this feature in the near future.

Additionally, I was unable to utilize a Cart/Order Table. I was unsure how I would go about connecting the Users & Products to a Cart/Order, due to lack of planning in the database design stages. I will be sure to dedicate more time towards the planning stage, as it will help with development in the long run.

Other additional features that I hope to add to my project in the future are listed below.

### Updates to be made
* Authentication/Authorization of specific users
  * Admins can access everything
  * Users can view
    * Other Users Details
    * Product Details
  * Users can edit
    * Their own Details
    * Their own Product Details
  * “Order/Cart” feature (MVC building)
    * User can add a product (that does not belong to them) to an order
    * User can delete a product in an order
* Form validations

Thank you for taking a look at my passion project!

###References
* [Christine Bittle's Varsity MVP example](https://github.com/christinebittle/varsity_mvp)
* [How to add a search feature](https://www.youtube.com/watch?v=Slw-gs2vcWo&ab_channel=kudvenkat)
* [Generated Photos](https://generated.photos/)
