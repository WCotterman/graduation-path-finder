using System.Collections.Generic;


/// <summary>
/// Simple data structure for storing classes with prereqs
/// </summary>
namespace GPF.Domain.Models
{
    public class CourseTree
    {
        public int Count;
        int loops = 0;
        public CourseTreeNode Root;
        public CourseTreeNode Current;
        List<Course> ListResult = new List<Course>();
        //Maintain index of courses by ID, removes need to search tree directly.
        public List<int> Courses = new List<int>();
        public CourseTree(List<Course> requiredCourses)
        {
            Count = 0;
            List<Course> SortedCourses = requiredCourses;
            SortedCourses.Sort();

            //add each course to the 'tree'
            while (SortedCourses.Count > 0 && loops < 5)
            {
                foreach (Course Course in SortedCourses.ToArray())
                {
                    if (Course.Prerequisites.Count == 0 || ContainsAllCourses(Course.Prerequisites)) {
                        ListResult.Add(Course);
                        AddNode(new CourseTreeNode(this, Course));
                        Count += 1;
                        Courses.Add(Course.Id);
                        SortedCourses.Remove(Course);
                    }
                }
                loops++;
            }
        }
        public bool ContainsCourse(Course Course)
        {
            if (Courses.Contains(Course.Id))
                return true;
            return false;
        }
        public bool ContainsAllCourses(List<Course> Courses)
        {
            bool res = true;
            foreach (Course C in Courses)
            {
                if (!ContainsCourse(C))
                    res = false;
            }
            return res;
        }
        //Returns the node for a particular course
        public CourseTreeNode GetNode(CourseTreeNode curr, int CourseID)
        {
            if (curr.Course.Id == CourseID)
                return curr;
            else
            {
                foreach (CourseTreeNode Node in curr.Children)
                   return GetNode(Node, CourseID);
            }
            return Root;
        }
        //add a node to the tree
        public void AddNode(CourseTreeNode Node)
        {
            //Connect to parents
            if (Node.Course.Prerequisites.Count == 0) {
                Node.Parents.Add(getRoot());
                Root.Children.Add(Node);
            }
            else
                foreach (Course req in Node.Course.Prerequisites)
                {
                    CourseTreeNode Temp = GetNode(getRoot(), req.Id);
                    Node.Parents.Add(Temp);
                    Temp.Children.Add(Node);
                }
        }
        public List<Course> GetList()
        {
            
            return ListResult;
        }
        private void GetListHelper(CourseTreeNode Node)
        {
            foreach (CourseTreeNode Child in Node.Children)
            {
                GetListHelper(Child);
            }
            ListResult.Add(Node.Course);

        }
        public CourseTreeNode getRoot()
        {
            if (Root == null)
            {
                //Create a dummy course to serve as the root
                Course dummy = new Course();
                dummy.Id = 00000;
                dummy.Number = 000;
                dummy.Department = "CSC";
                dummy.Title = "Root";
                dummy.Description = "Root course";
                dummy.Units = 0.0M;
                Root = new CourseTreeNode(this, dummy);
            }
            return Root;
        }
    }
}
