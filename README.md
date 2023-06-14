# Messaging App

This is a communication tool that allows users to connect and chat with one another quickly and conveniently. This README provides an overview of the app's features like AI message suggestions powered by OpenAI, Partial username searching to more easily find one another, and more.

## Table of Contents

- [Setup](#setup)
- [Usage](#usage)
    - [User Login and Signup](#user-login-and-signup)
    - [Partial Username Search and Messaging](#partial-username-search-and-messaging)
    - [AI Message Suggestions](#ai-message-suggestions)
    - [Error Handling](#error-handling)

## Setup
1. Clone project
2. Create an appsettings.json by copying appsettings.template.json
3. CD into API, then execute 'dotnet run'
4. CD into react-client, then execute 'npm start'
5. Client should be available at [http://localhost:3000](http://localhost:3000)

## Usage

### User Login and Signup
![msedge_4A1aRpYtC9](https://github.com/giannimperez/MessagingApp/assets/36053371/ee597112-71e5-42a0-b7ee-4b7e632e9e37)
<p align="center">Once logged in, a user is issued a token to be used for all subsequent requests.</p>

### Partial Username Search and Messaging
![msedge_FwZRQCI67o](https://github.com/giannimperez/MessagingApp/assets/36053371/e6870dfc-ece6-4845-bbc9-ece7b711a489)
<p align="center">Use the live search bar to locate users to chat with.</p>

### AI Message Suggestions
![msedge_lCdH7XNcTA](https://github.com/giannimperez/MessagingApp/assets/36053371/866bc01b-f7d5-4485-83de-f9b5f0d453a4)
<p align="center">Use the AI Message Suggestion button (powered by OpenAi) to create a message suggestion, which can be tweaked before sending.</p>
<p align="center">Message suggestions are created with the context of previous messages in order to provide more useful suggestions.</p>

### Error Handling
#### Toast for 400 responses:
![msedge_OLaWTelxkt](https://github.com/giannimperez/MessagingApp/assets/36053371/74b7db7f-0455-41b5-94f5-0353c729e4f6)

#### Toast for 500 responses:
![msedge_afa6jC2E41](https://github.com/giannimperez/MessagingApp/assets/36053371/fa2d024c-208b-46ff-a928-09b02c7f2fa3)
<p align="center">When a 500 occurs, Serilog is used to log the error with it's error guid to provide an easy method of troubleshooting.</p>

## Documentation
### Database Diagram:
![image](https://github.com/giannimperez/MessagingApp/assets/36053371/9fe4488f-6d20-4bbf-8092-c3a3cacd3ec4)

### API Request Flow Chart:
![Flowchart (5) drawio](https://github.com/giannimperez/MessagingApp/assets/36053371/22d19889-039e-426c-8cb5-c67a6e7526dc)

## Future Improvements

* Use SignalR for push notifications.
    * Would replace polling for messages
    * Would allow tracking of user activity (for "IsActive" flag)

* Track issued tokens with MongoDB.
    *  Would allow us to invalidate tokens when deleting a user.

* Implement a mechanism that limits OpenAI API requests to avoid extremely high charges.

## Credits
Developed by Gianni Perez
