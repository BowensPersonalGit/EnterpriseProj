import requests
from datetime import datetime, timedelta
import os

# Configuration
BASE_URL = "https://localhost:7202/api/AppointmentAPI"
PRACTITIONER_ID = 1                                               #==============Replace with actual ID===============

USER_API_URL = "https://localhost:7202/api/UserAPI"
practitioner_name = ""

VERIFY_SSL = False
APPT_LENGTH = timedelta(minutes=30)

def clear_screen():
    os.system('cls' if os.name == 'nt' else 'clear')


def input_date(prompt):
    date_input = input(f"{prompt} (YYYY-MM-DD): ").strip()
    try:
        return datetime.strptime(date_input, "%Y-%m-%d").date()
    except ValueError:
        print("❌ Invalid date format. Try again (YYYY-MM-DD).")
        return input_date(prompt)

def create_appointment(start, end):
    payload = {
        "startTime": start.isoformat(),
        "endTime": end.isoformat(),
        "pactionerId": PRACTITIONER_ID
    }

    try:
        response = requests.post(f"{BASE_URL}/create", json=payload, verify=VERIFY_SSL)
        response.raise_for_status()
        print(f"✅ Created: {start.strftime('%H:%M')} - {end.strftime('%H:%M')}")
    except requests.RequestException as e:
        print(f"❌ Failed: {start.strftime('%H:%M')} - {end.strftime('%H:%M')} | {e}")

def schedule_day():
    print("\n📆 Create practitioner schedule")

    # Gather inputs
    work_date = input_date("Enter date for the schedule")
    start_input = input("Enter start time (e.g., 08:00): ").strip()
    end_input = input("Enter end time (e.g., 16:30): ").strip()
    lunch_start_input = input("Enter lunch start time (e.g., 12:00): ").strip()
    lunch_duration_input = input("Enter lunch duration in minutes (e.g., 60): ").strip()

    try:
        start_time = datetime.combine(work_date, datetime.strptime(start_input, "%H:%M").time())
        end_time = datetime.combine(work_date, datetime.strptime(end_input, "%H:%M").time())
        lunch_start = datetime.combine(work_date, datetime.strptime(lunch_start_input, "%H:%M").time())
        lunch_duration = timedelta(minutes=int(lunch_duration_input))
        lunch_end = lunch_start + lunch_duration
    except ValueError:
        print("❌ Invalid time or duration input.")
        return

    # Generate schedule
    print(f"\n📅 Scheduling {work_date} from {start_input} to {end_input} with lunch from {lunch_start.strftime('%H:%M')} to {lunch_end.strftime('%H:%M')}\n")
    current = start_time
    while current + APPT_LENGTH <= end_time:
        if lunch_start <= current < lunch_end:
            print(f"🍽 Skipping: {current.strftime('%H:%M')} - {(current + APPT_LENGTH).strftime('%H:%M')} (Lunch)")
        else:
            create_appointment(current, current + APPT_LENGTH)
        current += APPT_LENGTH

def get_practitioner_appointments():
    try:
        response = requests.get(f"{BASE_URL}/list/byPractitioner/{PRACTITIONER_ID}", verify=VERIFY_SSL)
        response.raise_for_status()
        return response.json().get("appointments", [])
    except requests.RequestException as e:
        print(f"❌ Could not retrieve appointments: {e}")
        return []

def print_schedule_for_date():
    print("\n📋 View schedule for a specific date")
    work_date = input_date("Enter date to view schedule")

    appts = get_practitioner_appointments()
    found = False

    print(f"\n📅 Appointments for {work_date}\n----------------------------")
    for a in appts:
        start = datetime.fromisoformat(a["startTime"])
        end = datetime.fromisoformat(a["endTime"])

        if start.date() == work_date:
            found = True
            time_range = f"{start.strftime('%H:%M')} - {end.strftime('%H:%M')}"
            client = a.get("clientName")
            if client:
                print(f"📌 {time_range} : Booked by {client}")
            else:
                print(f"🟢 {time_range} : Available")

    if not found:
        print("🕒 No appointments found for this date.")


def get_practitioner_info(user_id):
    global practitioner_name

    try:
        response = requests.get(f"{USER_API_URL}/{user_id}", verify=VERIFY_SSL)
        response.raise_for_status()
        data = response.json()
        practitioner_name = data.get("name", "Practitioner")
        job = data.get("jobName", "")
        role = data.get("role", "")
        print(f"✅ Logged in as {practitioner_name} ({role} - {job})")
    except requests.RequestException as e:
        practitioner_name = "Practitioner"
        print(f"❌ Could not retrieve user info: {e}")


def main():
    global PRACTITIONER_ID

    clear_screen()
    try:
        PRACTITIONER_ID = int(input("🔑 Enter your Practitioner User ID: ").strip())
        get_practitioner_info(PRACTITIONER_ID)
    except ValueError:
        print("❌ Invalid ID. Please enter a number.")
        return

    while True:
        clear_screen()
        print(f"👨‍⚕️ Hello {practitioner_name}, welcome to your Scheduler")
        print("-------------------------------------------")
        print("1. Create schedule for a date")
        print("2. View schedule for a date")
        print("3. Exit")

        choice = input("\nEnter your choice (1-3): ").strip()

        if choice == "1":
            schedule_day()
        elif choice == "2":
            print_schedule_for_date()
        elif choice == "3":
            print("👋 Bye!")
            break
        else:
            print("❌ Invalid choice. Please try again.")
        input("\n🔁 Press Enter to return to the main menu...")

if __name__ == "__main__":
    main()
