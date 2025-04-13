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
          navText="Trang chủ"
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
            <i class="ni ni-pin-3 text-warning text-sm opacity-10"></i>
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
          to="/billing"
          :class="getRoute() === 'billing' ? 'active' : ''"
          navText="Vị trí truyền thông"
        >
          <template v-slot:icon>
            <i class="ni ni-image text-success text-sm opacity-10"></i>
          </template>
        </sidenav-item>
      </li>

      <li class="mt-3 nav-item">
        <h6 class="text-xs ps-4 text-uppercase font-weight-bolder opacity-6 ms-2">
          TÀI KHOẢN
        </h6>
      </li>

      <li class="nav-item">
        <sidenav-item
          to="/profile"
          :class="getRoute() === 'profile' ? 'active' : ''"
          navText="Hồ sơ"
        >
          <template v-slot:icon>
            <i class="ni ni-single-02 text-info text-sm opacity-10"></i>
          </template>
        </sidenav-item>
      </li>

      <li class="nav-item">
        <sidenav-item
          to="/login"
          :class="getRoute() === 'login' ? 'active' : ''"
          navText="Đăng xuất"
        >
          <template v-slot:icon>
            <i class="ni ni-key-25 text-danger text-sm opacity-10"></i>
          </template>
        </sidenav-item>
      </li>
    </ul>
  </div>
</template>
