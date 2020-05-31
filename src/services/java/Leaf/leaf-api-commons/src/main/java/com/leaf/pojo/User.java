package com.leaf.pojo;

public class User {

    private String UserId;
    private String UserName;
    private String Password;
    private boolean UserType;

    @Override
    public String toString() {
        return "User{" +
                "UserId='" + UserId + '\'' +
                ", UserName='" + UserName + '\'' +
                ", Password='" + Password + '\'' +
                ", UserType=" + UserType +
                '}';
    }

    public User() {
    }

    public User(String userId, String userName, String password, boolean userType) {
        UserId = userId;
        UserName = userName;
        Password = password;
        UserType = userType;
    }

    public String getUserId() {
        return UserId;
    }

    public void setUserId(String userId) {
        UserId = userId;
    }

    public String getUserName() {
        return UserName;
    }

    public void setUserName(String userName) {
        UserName = userName;
    }

    public String getPassword() {
        return Password;
    }

    public void setPassword(String password) {
        Password = password;
    }

    public boolean isUserType() {
        return UserType;
    }

    public void setUserType(boolean userType) {
        UserType = userType;
    }
}
