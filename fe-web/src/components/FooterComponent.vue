<template>
    <footer class="bg-green-800 text-white py-10 px-6">
    <div class="max-w-6xl mx-auto grid grid-cols-1 md:grid-cols-4 gap-8">
        <!-- Logo & Description -->
        <div>
        <div class="flex items-center gap-3 mb-4">
            <div class="bg-white rounded-full w-10 h-10 flex items-center justify-center">
            <span class="text-green-800 font-bold text-[17px]">TN</span>
            </div>
            <h2 class="font-bold text-lg">Thái Nguyên Travel</h2>
        </div>
        <p class="text-sm">
            Discover the hidden gems of Thái Nguyên, Vietnam with our curated travel guides, local insights, and personalized recommendations.
        </p>
        <div class="flex space-x-4 mt-4">
            <i class="fab fa-facebook-f"></i>
            <i class="fab fa-instagram"></i>
            <i class="fab fa-twitter"></i>
        </div>
        </div>

        <!-- Quick Links -->
        <div>
            <h3 class="font-bold mb-3">Quick Links</h3>
            <ul class="space-y-2 text-sm">
                <li><router-link to="/" class="hover:underline">Home</router-link></li>
                <li><router-link to="/blog" class="hover:underline">Travel Blog</router-link></li>
                <li><router-link to="/destinations" class="hover:underline">Destinations</router-link></li>
                <li><router-link to="/about" class="hover:underline">About Us</router-link></li>
                <li><router-link to="/contact" class="hover:underline">Contact</router-link></li>
            </ul>
        </div>

        <!-- Categories -->
        <div>
        <h3 class="font-bold mb-3">Categories</h3>
        <ul class="space-y-2 text-sm">
            <li>Nature</li>
            <li>Culture</li>
            <li>Food</li>
            <li>Adventure</li>
            <li>Relaxation</li>
        </ul>
        </div>

        <!-- Contact Info -->
        <div>
        <h3 class="font-bold mb-3">Contact Us</h3>
        <ul class="space-y-3 text-sm">
            <li class="flex items-start gap-2">
            <i class="fas fa-map-marker-alt mt-1"></i>
            123 Travel Street, Thái Nguyên City, Vietnam
            </li>
            <li class="flex items-center gap-2">
            <i class="fas fa-phone-alt"></i>
            +84 123 456 789
            </li>
            <li class="flex items-center gap-2">
            <i class="fas fa-envelope"></i>
            info@thainguyentravel.com
            </li>
        </ul>
        </div>
    </div>

    <!-- Copyright -->
    <div class="text-center text-xs text-gray-300 mt-10">
        &copy; 2025 Thái Nguyên Travel. All rights reserved.
    </div>
    </footer>
</template>

<script setup>
// import { ref } from 'vue';
</script>

<style scoped>
/* Add Font Awesome icons (if not already included in project) */
@import url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css');
</style>
