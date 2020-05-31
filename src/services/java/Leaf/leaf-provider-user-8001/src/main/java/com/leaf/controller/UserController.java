package com.leaf.controller;

import com.leaf.pojo.User;
import com.leaf.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
public class UserController {

    @Autowired
    private UserService userService;

    @GetMapping("/user/queryAllUsers")
    public List<User> queryAllUsers() {
        return userService.queryAllUsers();
    }

    @PostMapping("/user/addNewUser")
    @ResponseBody
    public Long addNewUser(@RequestBody User user) {
        System.out.println(user);
        Boolean boolAddUser = userService.addNewUser(user);
        return user.getUserId();
    }


}
