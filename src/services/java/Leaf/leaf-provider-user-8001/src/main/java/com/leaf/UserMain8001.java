package com.leaf;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.netflix.eureka.EnableEurekaClient;

@EnableEurekaClient
@SpringBootApplication
public class UserMain8001 {
    public static void main(String[] args) {
        SpringApplication.run(UserMain8001.class, args);
    }
}
