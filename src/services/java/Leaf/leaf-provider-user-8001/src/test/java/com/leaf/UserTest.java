package com.leaf;

import com.leaf.pojo.User;
import com.leaf.service.UserService;
import org.junit.Test;
import org.springframework.beans.factory.annotation.Autowired;

import java.util.List;

public class UserTest {

    @Autowired
    private UserService userService;

    @Test
    public void queryAllUsers(){
        List<User> users = userService.queryAllUsers();
        System.out.println(users);
    }

}
