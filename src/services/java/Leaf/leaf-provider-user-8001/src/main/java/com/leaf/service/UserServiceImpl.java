package com.leaf.service;

import cn.hutool.core.lang.Snowflake;
import cn.hutool.core.util.IdUtil;
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

    @Override
    public Boolean addNewUser(User user) {

        Snowflake snowflake = IdUtil.createSnowflake(1, 1);
        long id = snowflake.nextId();
        user.setUserId(id);

        int i = userMapper.addNewUser(user);
        if (i > 0) {
            return true;
        }
        return false;
    }


}
