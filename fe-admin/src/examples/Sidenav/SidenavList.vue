<script setup>
import { useRoute } from "vue-router";
// import { useStore } from "vuex";

import SidenavItem from "./SidenavItem.vue";

const getRoute = () => {
  const route = useRoute();
  const routeArr = route.path.split("/");
  return routeArr[1];
};
</script>

<template>
  <div
    class="collapse navbar-collapse w-auto h-auto h-100"
    id="sidenav-collapse-main"
  >
    <ul class="navbar-nav">
      <li class="nav-item">
        <sidenav-item
          to="/dashboard-default"
          :class="getRoute() === 'dashboard-default' ? 'active' : ''"
          navText="Báo cáo/Thống kê"
        >
          <template v-slot:icon>
            <i class="ni ni-shop text-primary text-sm opacity-10"></i>
          </template>
        </sidenav-item>
      </li>

      <li class="nav-item">
        <sidenav-item
          to="/add-place"
          :class="getRoute() === 'tables' ? 'active' : ''"
          navText="Thêm địa điểm"
        >
          <template v-slot:icon>
            <!-- <i class="ni ni-pin-3"></i> -->
            <i
              class="fa-solid fa-location-crosshairs text-warning text-sm opacity-10"
            ></i>
          </template>
        </sidenav-item>
      </li>

      <li class="nav-item">
        <sidenav-item
          to="/management-place"
          :class="getRoute() === 'tables' ? 'active' : ''"
          navText="Quản lý địa điểm"
        >
          <template v-slot:icon>
            <i class="ni ni-pin-3 text-warning text-sm opacity-10"></i>
          </template>
        </sidenav-item>
      </li>

      <li class="nav-item">
        <sidenav-item
          to="/management-blog"
          :class="getRoute() === 'tables' ? 'active' : ''"
          navText="Quản lý bài viết"
        >
          <template v-slot:icon>
            <i class="fa-solid fa-file text-warning text-sm opacity-10"></i>
          </template>
        </sidenav-item>
      </li>
      <li class="nav-item">
        <sidenav-item
          to="/management-user"
          :class="getRoute() === 'tables' ? 'active' : ''"
          navText="Quản lý người dùng"
        >
          <template v-slot:icon>
            <i class="fa-solid fa-users text-warning text-sm opacity-10"></i>
          </template>
        </sidenav-item>
      </li>
      <li class="nav-item">
        <sidenav-item
          to="/review"
          :class="getRoute() === 'tables' ? 'active' : ''"
          navText="Quản lý Đánh giá"
        >
          <template v-slot:icon>
            <i class="fa-solid fa-comment text-warning text-sm opacity-10"></i>
          </template>
        </sidenav-item>
      </li>
    </ul>
  </div>
</template>
