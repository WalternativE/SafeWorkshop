## SAFE Workshop

To build - `fake build`
To run (watch mode) - `fake build -t Run`

1. Add users model with Saturn tooling - `dotnet saturn gen.model User Users id:string username:string password:string role:string`
2. Add additional query to user repository
3. Add login and signup views
4. Create user handlers
5. Update index view to include user info

6. Add articles endpoint with Saturn tooling - `dotnet saturn gen Article Articles id:string author_id:string title:string date:string content:string`
7. Add article controller to the router
8. Use plugs to authorize editing articles
9. Handle return url

9. Add comments endpoint with Saturn tooling - `dotnet saturn gen Comment Comments id:string author_id:string article_id:string date:string content:string`
10. Add comments controller as subcontroller for the articles controller
11. Update comments controller

12. Add API (json) controllers for articles and comments

13. In the client side create the component for showing/editing/updating single comment
14. In the client side create the component for showing list of the comments for particular article
15. Create component for showing/editing/updating single article
16. Create component for showing list of articles

17. Create an API for external React component (editor control)
18. Plug external React component into article and comment controls

19. Go back to server side, use DI to get logger in the controller actions and use it to log all actions and errors
20. Configure logger in the program

21. Communication with Fable.Remoting
22. Testing
23. 3rd Party OAuth
24. Writing Saturn Extension
25. Hosting