# =========================================
# Stage 1: Build the Angular Application
# =========================================
# =========================================
# Stage 1: Build the Angular Application
# =========================================
ARG NODE_VERSION=22.17.1-alpine
ARG NGINX_VERSION=stable-alpine3.21

# Use a lightweight Node.js image for building (customizable via ARG)
FROM node:${NODE_VERSION} AS builder

# Set the working directory inside the container
WORKDIR /app

# Copy package-related files first to leverage Docker's caching mechanism
COPY package.json package-lock.json ./

# Install project dependencies using npm ci (ensures a clean, reproducible install)
# RUN --mount=type=cache,target=/root/.npm npm ci
RUN npm install --legacy-peer-deps

# Copy the rest of the application source code into the container
COPY . .

# Replace placeholder with API_URL in environment.ts during build
ARG API_URL=http://localhost:5254/api/
RUN sed -i "s|API_URL|${API_URL}|" /app/src/environments/environment.ts
RUN sed -i "s|API_URL|${API_URL}|" /app/src/environments/environment.prod.ts

# Build the Angular application
RUN npm run build --prod

# =========================================
# Stage 2: Prepare Nginx to Serve Static Files
# =========================================

FROM nginxinc/nginx-unprivileged:${NGINX_VERSION} AS runner

# Use a built-in non-root user for security best practices
USER nginx
# USER root

# Copy custom Nginx config
COPY nginx.conf /etc/nginx/nginx.conf

# Copy the static build output from the build stage to Nginx's default HTML serving directory
COPY --chown=nginx:nginx --from=builder /app/dist/client/browser /usr/share/nginx/html

# Expose port 8080 to allow HTTP traffic
# Note: The default NGINX container now listens on port 8080 instead of 80 
EXPOSE 8080

# Start Nginx directly with custom config
ENTRYPOINT ["nginx", "-c", "/etc/nginx/nginx.conf"]
CMD ["-g", "daemon off;"]