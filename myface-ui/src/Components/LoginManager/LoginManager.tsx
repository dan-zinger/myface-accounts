import React, {createContext, ReactNode, useState} from "react";


export const LoginContext = createContext({
    isLoggedIn: false,
    role: 0,
    username: "",
    setUserName: (username: string) => {},
    password: "",
    setPassword: (username: string) => {},
    logIn: () => {},
    logOut: () => {},
    setRole: (Role: number) => {},
    setUserId: (userId: number) => {},
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [username, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [loggedIn, setLoggedIn] = useState(false);
    const [role, setRole] = useState(0);
    const [userId, setUserId] = useState(0)
    
    function logIn() {
        setLoggedIn(true);
    }
    
    function logOut() {
        setLoggedIn(false);
    }

    const context = {
        isLoggedIn: loggedIn,
        role: role,
        username: username,
        setUserName: setUserName,
        password: password,
        setPassword: setPassword,
        logIn: logIn,
        logOut: logOut,
        setRole: setRole, 
        setUserId: setUserId,
        userId: userId
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}