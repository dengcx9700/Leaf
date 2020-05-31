package com.leaf.service;

import com.leaf.pojo.User;

import java.util.List;

public interface UserService {

    List<User> queryAllUsers();

    Boolean addNewUser(User user);
}
