package com.leaf.service;

import com.leaf.mapper.UserMapper;
import com.leaf.pojo.User;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class UserServiceImpl implements UserService {

    @Autowired
    private UserMapper userMapper;

    @Override
    public List<User> queryAllUsers() {

        List<User> users = userMapper.queryAllUsers();

        System.out.println(users);

        return users;
    }
}
