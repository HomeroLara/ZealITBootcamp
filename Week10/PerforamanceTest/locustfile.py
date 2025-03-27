from locust import HttpUser, task, between
import ssl

class CacheTestUser(HttpUser):
    wait_time = between(1, 3)  # Wait time between tasks

    def on_start(self):
        """This method is run when a simulated user starts."""
        # Disable SSL verification for self-signed cert (if needed)
        self.client.verify = False  # This disables SSL certificate verification

    @task
    def get_inmemorycache(self):
        response = self.client.get("/inmemorycache")  # Endpoint you've specified
        if response.status_code == 200:
            print(f"Response: {response.json()}")
        else:
            print(f"Error: {response.status_code}")

    @task
    def get_rediscache(self):
        response = self.client.get("/rediscache")  # Endpoint you've specified
        if response.status_code == 200:
            print(f"Response: {response.json()}")
        else:
            print(f"Error: {response.status_code}")

