package com.leaf.controller;

import com.leaf.pojo.User;
import com.leaf.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
public class UserController {

    @Autowired
    private UserService userService;

    @GetMapping("/user/queryAllUsers")
    public List<User> queryAllUsers(){
        return userService.queryAllUsers();
    }


}
